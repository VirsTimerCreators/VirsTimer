package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.security.access.annotation.Secured
import org.springframework.security.core.Authentication
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Event
import pl.virstimer.repository.EventCustomRepository
import pl.virstimer.repository.EventRepository
import java.util.*

@RestController
@RequestMapping("/event")

internal class EventController(val repository: EventRepository, val customRepository: EventCustomRepository) {

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

    @PatchMapping("/{eventId}")
    @Secured("ROLE_USER")
    fun updateEvent(@PathVariable eventId: String, @RequestBody event: EventRequest, authentication: Authentication)
    : ResponseEntity<Unit> {
        customRepository.updateEvent(eventId, event, authentication.name)
        return ResponseEntity.ok(Unit)
    }
    
    @DeleteMapping("/{eventId}")
    @Secured("ROLE_USER")
    fun deleteEventByIdAndUserId(@PathVariable eventId: String, authentication: Authentication) = repository.deleteEventByIdAndUserId(eventId, authentication.name)

    @DeleteMapping
    @Secured("ROLE_USER")
    fun deleteAllUserEvents(authentication: Authentication){
        repository.deleteAllByUserId(authentication.name)
    }
}

data class EventRequest(val puzzleType: String)