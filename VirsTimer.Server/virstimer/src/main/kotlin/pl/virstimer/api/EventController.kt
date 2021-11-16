package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.security.access.annotation.Secured
import org.springframework.security.core.Authentication
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Event
import pl.virstimer.repository.EventRepository
import java.util.*

@RestController
@RequestMapping("/event")
internal class EventController(val repository: EventRepository) {

    @GetMapping
    @Secured("ROLE_USER")
    fun findAllForUser(authentication: Authentication): List<Event> = repository.findByUserId(authentication.name)

    @PostMapping
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

    @DeleteMapping("/{eventId}")
    @Secured("ROLE_USER")
    fun deleteEventById(@PathVariable eventId: String) = repository.deleteEventById(eventId)

    @DeleteMapping
    @Secured("ROLE_USER")
    fun deleteAllUserEvents(authentication: Authentication){
        repository.deleteAllByUserId(authentication.name)
    }
}

data class EventRequest (val puzzleType: String)