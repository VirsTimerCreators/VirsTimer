package pl.virstimer.model.multiplayer

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document
import pl.virstimer.model.Solved

@Document("multiplayer_solves")
data class MultiplayerSolve(
    @Id
    val id: String,
    val userId: String,
    val roomId: String,
    val scrambleId: String,
    val time: Long,
    val timestamp: Long,
    val solved: Solved
)

