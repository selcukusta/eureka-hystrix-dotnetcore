# Using Netflix/Hystrix with .NET Core Applications

Repository has these applications;

> `/eureka` -> Use the Service Registry to dynamically discover and call registered services. Developed by Netflix.

> `/hystrix-dashboard` -> A latency and fault tolerance library. Developed by Netflix.

> `/custom-sample-api` -> Provides sample data and registered to `eureka`. Can be called any application which fetchs registered services from it as `http://remote-service`.

> `/custom-sample-ui` -> Circuit breaker pattern is implemented. All metrics can be discovered with Hystrix Dashboard. It calls `custom-sample-api`.

## Requirements:
- JDK 1.8
- Apache Maven 3.5.4
- dotnet 2.1 

## Run

### Eureka

```bash
cd /eureka
mvn -N io.takari:maven:wrapper
./mvnw spring-boot:run
```

### Hystrix Dashboard

```bash
cd /hystrix-dashboard
mvn install
java -jar target/hystrix-dashboard-0.0.1.BUILD-SNAPSHOT.jar
```

### Sample API

```bash
cd /custom-sample-api
dotnet run
```

### Sample UI

```bash
cd /custom-sample-ui
dotnet run
```

## Tests
Start UI application and run `while true; do sleep 1; curl -s http://localhost:1923/api/home;echo -e '\r';done` on terminal. *Note: API is not running yet!* 

After executes less than 10 requests (circuitBreaker:RequestVolumeThreshold, default to 20) in 10 seconds (metrics:rollingStats:timeInMilliseconds, default to 10000 seconds), there won't be any decision to change the status. If fulfills the minimum threshold requirement and more than 80% of requests have failed (circuitBreaker:errorThresholdPercentage, default to 50), Hystrix will decide to open the Circuit Breaker.

When you see 

```diff
Circuit Open
```
message on `http://localhost:7979/hystrix/monitor?stream=http://localhost:1923/hystrix/hystrix.stream` run the API immediately and wait until status changes!

## Credits
- [Netflix/Hystrix](https://github.com/Netflix/Hystrix)
- [Steeltoe/Circuit Breaker](https://github.com/SteeltoeOSS/CircuitBreaker)
- [Kubrynski - Overview of the circuit breaker in Hystrix](http://www.kubrynski.com/2017/07/overview-of-circuit-breaker-in-hystrix.html)
