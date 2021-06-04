package pl.virstimer.repository

import org.bson.types.ObjectId
import org.springframework.boot.CommandLineRunner
import org.springframework.context.annotation.Bean
import org.springframework.data.mongodb.repository.MongoRepository
import pl.virstimer.model.Event

interface EventRepository : MongoRepository<Event, String> {
    fun findOneById(id: ObjectId): Event
    override fun deleteAll()
    fun findById(id: ObjectId): Iterable<Event>

    }