package pl.virstimer.repository

import org.bson.types.ObjectId
import org.springframework.boot.CommandLineRunner
import org.springframework.context.annotation.Bean
import org.springframework.data.mongodb.repository.MongoRepository
import pl.virstimer.model.Event

interface EventRepository : MongoRepository<Event, String> {
    override fun deleteAll()
    fun findByUserId(userId: String): List<Event>
    fun deleteEventByIdAndUserId(id: String, userId: String)
    fun deleteAllByUserId(userId: String)
}