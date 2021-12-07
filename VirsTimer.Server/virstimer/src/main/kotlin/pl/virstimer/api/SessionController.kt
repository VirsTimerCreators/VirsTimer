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

    @GetMapping
    @Secured("ROLE_USER")
    fun findUserSessions(authentication: Authentication): List<Session> = repository.findAllByUserId(authentication.name)

    @GetMapping("/{sessionId}")
    @Secured("ROLE_USER")
    fun getSession(@PathVariable sessionId: ObjectId, authentication: Authentication): Session = repository.findOneByIdAndUserId(sessionId, authentication.name)

    @GetMapping("/event/{eventId}")
    @Secured("ROLE_USER")
    fun findSessionsForEvent(@PathVariable eventId: String, authentication: Authentication): List<Session> = repository.findByEventIdAndUserId(eventId, authentication.name)

    @PostMapping
    @Secured("ROLE_USER")
    fun createSession(@RequestBody request: SessionRequest,authentication: Authentication): ResponseEntity<Session> {
        val session = repository.save(
            Session(
                userId = authentication.name,
                eventId = request.eventId,
                name = request.name,
                id = UUID.randomUUID().toString()
            )
        )
        return ResponseEntity(session, HttpStatus.CREATED)
    }

    @PatchMapping("/{sessionId}")
    @Secured("ROLE_USER")
    fun updateSession(@PathVariable sessionId: String, @RequestBody session: SessionChange, authentication: Authentication): ResponseEntity<Unit> {
        customRepository.updateSession(sessionId, session, authentication.name)

        return ResponseEntity.ok(Unit)
    }

    @DeleteMapping("/{sessionId}")
    @Secured("ROLE_USER")
    fun deleteSessionByIdAndUserId(@PathVariable sessionId: String, authentication: Authentication) = repository.deleteSessionByIdAndUserId(sessionId, authentication.name)

    @DeleteMapping
    @Secured("ROLE_USER")
    fun deleteAllUserSessions(authentication: Authentication){
       repository.deleteAllByUserId(authentication.name)
    }
}

data class SessionRequest(
    val eventId: String,
    val name: String
)

data class SessionChange(val name:String)