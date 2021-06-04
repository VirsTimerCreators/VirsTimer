package pl.virstimer.model

import org.bson.types.ObjectId
import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document


@Document("sessions")
class Session(
    @Id
    var id: ObjectId?=null,
    var userId: String,
    var eventId: String,
    var name: String,
)

data class SessionChange(val name:String)