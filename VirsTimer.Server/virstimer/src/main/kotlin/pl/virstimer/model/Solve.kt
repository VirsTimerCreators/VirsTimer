package pl.virstimer.model

import org.bson.types.ObjectId
import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document

//{ "_id": "ObjectId", "userId": "String", "sessionId": "String", "scramble": "String", "time": "Long",
// "timestamp": "Long", // ms, "solved": [ "OK", "PLUS_TWO", "DNF" ] // enum }
@Document("solves")
class Solve(
    @Id
    var id: ObjectId?=null,
    var userId: String,
    var sessionId: String,
    var scramble: String,
    var time: Long,
    var timestamp: Long,
    var solved: Solved
)

data class SolveChange(val solved:Solved)

enum class Solved {
    OK,
    PLUS_TWO,
    DNF
}
