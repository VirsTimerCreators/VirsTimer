package pl.virstimer.api

import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.security.access.prepost.PreAuthorize
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

    @GetMapping("/user/{userId}")
    @PreAuthorize("hasRole('USER')")
    fun findAllForUser(@PathVariable userId: String): List<Session> = repository.findAllByUserId(userId)

    @GetMapping("/{sessionId}")
    @PreAuthorize("hasRole('USER')")
    fun getSession(@PathVariable sessionId: String): Session = repository.findOneById(sessionId)

    @GetMapping("/event/{eventId}/user/{userId}")
    @PreAuthorize("hasRole('USER')")
    fun findAllForEventIdAndUserId(@PathVariable eventId: String, @PathVariable userId: String): List<Session> = repository.findByEventIdAndUserId(eventId, userId)

    @PostMapping
    @PreAuthorize("hasRole('USER')")
    fun createSession(@RequestBody request: SessionRequest): ResponseEntity<Session> {
        val session = repository.save(
            Session(
                id = UUID.randomUUID().toString(),
                userId = request.userId,
                eventId = request.eventId,
                name = request.name
            )
        )
        return ResponseEntity(session, HttpStatus.CREATED)
    }


    @PatchMapping("/{sessionId}")
    @PreAuthorize("hasRole('USER')")
    fun updateSession(@PathVariable sessionId: String, @RequestBody sessionChange: SessionChange): ResponseEntity<Unit> {
        customRepository.updateSession(sessionId, sessionChange)

        return ResponseEntity.ok(Unit)
    }

    @DeleteMapping("/{sessionId}")
    fun deleteSession(@PathVariable sessionId: String) = repository.deleteSessionById(sessionId)
}

data class SessionRequest(
    val userId: String,
    val eventId: String,
    val name: String
)

data class SessionChange(val name:String)