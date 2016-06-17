// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open MassTransit
open System
open MassTransitStudies.Messages
let configureBus()=
    Bus.Factory.CreateUsingRabbitMq(fun cfg ->
                cfg.Host(new Uri("rabbitmq://localhost"), (fun h ->
                    h.Username("guest")
                    h.Password("guest")
                )) |> ignore)

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    let busControl = configureBus()
    using(busControl.Start())
        (fun h->
            let mutable run = true
            while run do
                printfn "Enter message (or quit to exit)" 
                printfn "> "
                let value = Console.ReadLine()
                match value with
                | "quit" -> run<-false
                | _ ->busControl.Publish<ValueEntered>({ Value=value }) |> ignore
            busControl.Stop() |> ignore
        )
    0 // return an integer exit code

