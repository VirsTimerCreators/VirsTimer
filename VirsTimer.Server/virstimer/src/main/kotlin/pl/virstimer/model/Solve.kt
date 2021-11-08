package pl.virstimer.model

import com.fasterxml.jackson.annotation.JsonIgnoreProperties
import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document

@Document("solves")
data class Solve(
    @Id
    val id: String,
    val userId: String,
    val sessionId: String,
    val scramble: String,
    val time: Long,
    val timestamp: Long,
    val solved: Solved
)

@JsonIgnoreProperties()
data class SolveChange(val solved:Solved)

enum class Solved {
    OK,
    PLUS_TWO,
    DNF
}
