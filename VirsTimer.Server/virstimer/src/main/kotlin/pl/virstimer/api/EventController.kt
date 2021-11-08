package pl.virstimer.api

import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.security.access.annotation.Secured
import org.springframework.security.core.Authentication
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Event
import pl.virstimer.repository.EventRepository
import java.util.*

@RestController
internal class EventController(val repository: EventRepository) {


    @GetMapping("/event/user/{userId}")
    fun findAllForUser(@PathVariable userId: String): List<Event> = repository.findByUserId(userId)

    @PostMapping("/events")
    @Secured("ROLE_USER")
    fun createEvent(@RequestBody request: EventRequest, authentication: Authentication): ResponseEntity<Event> {
        val event = repository.save(
            Event(
                id = UUID.randomUUID().toString(),
                userId = authentication.name,
                puzzleType = request.puzzleType
            )
        )
        return ResponseEntity(event, HttpStatus.CREATED)
    }
}

data class EventRequest (val puzzleType: String)