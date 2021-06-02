package pl.virstimer.model

import org.bson.types.ObjectId
import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document



@Document("sessions")
class Session (
    @Id
    val id: ObjectId,
    val userId: String,
    val eventId: String,
    val name: String,
)