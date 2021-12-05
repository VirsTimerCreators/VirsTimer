package pl.virstimer.service

import org.springframework.stereotype.Component
import pl.virstimer.model.Event
import pl.virstimer.repository.EventRepository
import java.util.*

@Component
class EventService(val eventRepository: EventRepository) {
    fun createEvents(username: String) {
        val events = mutableListOf<Event>()
        val puzzleTypes = listOf("2x2x2", "3x3x3", "4x4x4", "5x5x5", "6x6x6", "7x7x7", "3x3x3 BlindFolded", "3x3x3 OH",
            "Clock", "Megaminx", "Pyraminx", "Skweb", "Square One", "4x4x4 BlindFolded", "5x5x5 BlindFolded")
        puzzleTypes.forEach {
            events.add(
                Event(
                    id = UUID.randomUUID().toString(),
                    userId = username,
                    puzzleType = it
                )
            )
        }
        eventRepository.saveAll(events)
    }
}