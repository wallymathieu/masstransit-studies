open MassTransit
open System
open System.Threading.Tasks
open MassTransitStudies.Messages

type ValueEnteredConsumer()=
    interface IConsumer<ValueEntered> with
        member this.Consume c=
            printfn "Value entered %s" c.Message.Value
            Task.FromResult(0) :> Task


let configureBus()=
    Bus.Factory.CreateUsingRabbitMq(fun cfg ->
                let host = cfg.Host(new Uri("rabbitmq://localhost"), (fun h ->
                    h.Username("guest")
                    h.Password("guest")
                )) 
                cfg.ReceiveEndpoint(host, "subscriber", fun e->e.Consumer<ValueEnteredConsumer>())
                )

[<EntryPoint>]
let main argv = 
    let busControl = configureBus()
    let mutable run = true
    using(busControl.Start())
        (fun h->
            printfn "Press something to finish" 
            try
                Console.ReadLine() |> ignore
            finally
               busControl.Stop()
        )
    0 // return an integer exit code

