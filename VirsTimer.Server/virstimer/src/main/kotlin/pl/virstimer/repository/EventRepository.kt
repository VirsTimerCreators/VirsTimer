package pl.virstimer.repository

import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.core.query.Update
import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.api.EventRequest
import pl.virstimer.model.Event

interface EventRepository : MongoRepository<Event, String> {
    override fun deleteAll()
    fun findByUserId(userId: String): List<Event>
}


interface EventCustomRepository {
    fun updateEvent(id: String, eventUpdate: EventRequest, userId: String)
}

@Repository
class EventCustomRepositoryImpl(val mongoTemplate: MongoTemplate) : EventCustomRepository {
    override fun updateEvent(id: String, eventUpdate: EventRequest, userId: String) {
        mongoTemplate.updateFirst(
            Query(Criteria("_id").`is`(id).and("userId").`is`(userId)),
            Update().set("puzzleType", eventUpdate.puzzleType),
            Event::class.java
        )
    }
}