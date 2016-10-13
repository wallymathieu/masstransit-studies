// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open MassTransit
open System
open MassTransitStudies.Messages
open MassTransit.QuartzIntegration
let configureBus()=
    Bus.Factory.CreateUsingRabbitMq(fun cfg ->
                cfg.Host(new Uri("rabbitmq://localhost"), (fun h ->
                    h.Username("guest")
                    h.Password("guest")
                )) |> ignore
                cfg.UseInMemoryScheduler()
                )

[<EntryPoint>]
let main argv = 
    let busControl = configureBus()
    let mutable run = true
    using(busControl.Start())
        (fun h->
            printfn "Enter message (or ctrl+c to exit)" 
            try
                while run do
                    printf "> "
                    let value = Console.ReadLine()
                    if not(String.IsNullOrEmpty(value)) then
                        let sendEndpoint =  busControl.GetSendEndpoint(Uri("rabbitmq://localhost/quartz"))
                        let s=sendEndpoint.Result.ScheduleSend<DelayedMessage>(Uri("rabbitmq://localhost/service_queue"),
                                                                        TimeSpan.FromSeconds(1.),
                                                                        {DelayedValue=value},Threading.CancellationToken.None)
                        s.Wait()
                        busControl.Publish<ValueEntered>({ Value=value }) |> ignore
                        printfn "published %s" value 
                    else
                        ()
            finally
               busControl.Stop()
        )
    0 // return an integer exit code

