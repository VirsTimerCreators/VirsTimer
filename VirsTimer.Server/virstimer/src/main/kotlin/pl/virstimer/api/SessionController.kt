package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.security.access.annotation.Secured
import org.springframework.security.core.Authentication
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Session
import pl.virstimer.repository.SessionCustomRepository
import pl.virstimer.repository.SessionRepository
import java.util.*

@RestController
@RequestMapping("/session")
class SessionController(
    val repository: SessionRepository,
    val customRepository: SessionCustomRepository
) {

    @GetMapping("/all")
    @Secured("ROLE_USER")
    fun findAllSessions(authentication: Authentication): List<Session> = repository.findAllByUserId(authentication.name)

    @GetMapping("/{sessionId}")
    @Secured("ROLE_USER")
    fun getSession(@PathVariable sessionId: ObjectId, authentication: Authentication): Session = repository.findOneByIdAndUserId(sessionId, authentication.name)

    @GetMapping("/user")
    @Secured("ROLE_USER")
    fun findAllUserId(authentication: Authentication): List<Session> = repository.findAllByUserId(authentication.name)

    @GetMapping("/event/{eventId}")
    @Secured("ROLE_USER")
    fun findAllEventId(@PathVariable eventId: String, authentication: Authentication): List<Session> = repository.findByEventIdAndUserId(eventId, authentication.name)

    @GetMapping("/event/{eventId}/user/{userId}")
    @Secured("ROLE_USER")
    fun findAllForEventIdAndUserId(@PathVariable eventId: String, @PathVariable userId: String): List<Session> = repository.findByEventIdAndUserId(eventId, userId)

    @PostMapping("/post")
    @Secured("ROLE_USER")
    fun createSession(@RequestBody request: SessionRequest): ResponseEntity<Session> {
        val session = repository.save(
            Session(
                userId = request.userId,
                eventId = request.eventId,
                name = request.name,
                id = UUID.randomUUID().toString()
            )
        )
        return ResponseEntity(session, HttpStatus.CREATED)
    }

    @PatchMapping("/patch/{sessionId}")
    @Secured("ROLE_USER")
    fun updateSession(@PathVariable sessionId: ObjectId, @RequestBody session: SessionChange, authentication: Authentication): ResponseEntity<Unit> {
        customRepository.updateSession(sessionId, session, authentication.name)

        return ResponseEntity.ok(Unit)
    }

    @DeleteMapping("delete/{sessionId}")
    @Secured("ROLE_USER")
    fun deleteSession(@PathVariable sessionId: ObjectId) = repository.deleteSessionById(sessionId)

    @DeleteMapping("delete/all")
    @Secured("ROLE_USER")
    fun deleteSession(authentication: Authentication){
       repository.deleteAllByUserId(authentication.name)
    }
}

data class SessionRequest(
    val userId: String,
    val eventId: String,
    val name: String
)

data class SessionChange(val name:String)