package pl.virstimer.model.multiplayer

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document
import pl.virstimer.domain.PuzzleType

@Document("scrambles")
data class PersistentScramble(
    @Id
    val id: String,
    val scrambleText: String,
    val scrambleSvg: String,
    val generatedAt: Long,
    val puzzleType: PuzzleType
)