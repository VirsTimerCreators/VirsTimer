package pl.virstimer.model

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document

@Document("events")
data class Event(
    @Id
    val id: String,
    val userId: String,
    val puzzleType: String
)