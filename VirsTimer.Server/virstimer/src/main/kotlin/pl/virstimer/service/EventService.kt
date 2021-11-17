package pl.virstimer.service

import org.springframework.stereotype.Component
import pl.virstimer.domain.PuzzleType
import pl.virstimer.model.Event
import pl.virstimer.repository.EventRepository
import java.util.*

@Component
class EventService(val eventRepository: EventRepository) {
    fun createEvents( username: String){
        enumValues<PuzzleType>().forEach {
            eventRepository.save(
                Event(
                    id = UUID.randomUUID().toString(),
                    userId = username,
                    puzzleType = it.name
                )
            )
        }
    }
}