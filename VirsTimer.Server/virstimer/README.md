## Virstimer.Server

### 

*To run application locally, a mongo application should be available on port 27017*

To run program use gradle wrapper (gradlew or gradlew.bat depending on your OS)

```./gradlew.bat bootRun``` on windows, or
```./gradlew bootRun``` should start the application


To verify that application is running successfully 

``` curl -I localhost:8080/actuator/info```

should return something like this:
```
HTTP/1.1 200
Content-Type: application/vnd.spring-boot.actuator.v3+json
Content-Length: 2
Date: Sat, 29 May 2021 14:56:54 GMT
```