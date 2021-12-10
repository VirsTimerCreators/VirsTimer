package pl.virstimer.model.multiplayer

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document
import pl.virstimer.api.multiplayer.RoomStatus

@Document("rooms")
data class PersistentRoom(
    @Id
    val id: String,
    val scrambleIds: Set<String>,
    val status: RoomStatus,
    val administratorId: String,
    val users: Set<String>,
    val joinCode: String
)