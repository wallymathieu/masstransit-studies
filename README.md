# masstransit-studies

## Howto run

Use docker-compose to start rabbitMQ, site and subscriber service.

> docker-compose up

Use command line app ```Generator``` to generate messages. These messages should now be present on the site.

> dotnet run --project ./Generator/

Goto site on localhost:

> start http://localhost:5100/

## What

This is mainly in order to demonstrate howto get started with Masstransit.

Masstransit (and similar) allows you to wire up loosely coupled distributed [mediator](https://en.wikipedia.org/wiki/Mediator_pattern).
