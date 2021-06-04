package pl.virstimer

import com.fasterxml.jackson.databind.ObjectMapper
import com.google.gson.Gson
import org.bson.types.ObjectId
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.http.MediaType
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import pl.virstimer.api.SessionRequest
import pl.virstimer.model.*
import org.springframework.test.web.servlet.ResultActions
import pl.virstimer.api.auth.AuthControllerIntTest
import pl.virstimer.security.LoginRequest
import pl.virstimer.security.SignupRequest

open class TestCommons {
    @Autowired
    lateinit var mongoTemplate: MongoTemplate

    @Autowired
    lateinit var mockMvc: MockMvc

    @Autowired
    lateinit var objectMapper: ObjectMapper

    fun before_each() {
        mongoTemplate.dropCollection(Event::class.java)
        mongoTemplate.dropCollection(Session::class.java)
        mongoTemplate.dropCollection(Solve::class.java)
        mongoTemplate.insertAll(
            listOf(
                Event(null, "1", "THREE_BY_THREE"),
                Event(null, "2", "FOUR_BY_FOUR"),
                Session(null, "1", "1", "session1"),
                Session(null, "1", "2", "session2"),
                Solve(null, "1", "1", "11111", 5125215, 5315541, Solved.DNF),
                Solve(null, "1", "1", "22222", 51252315, 53515241, Solved.PLUS_TWO),
                Solve(null, "2", "2", "33333", 512522315, 53152441, Solved.DNF),
                Solve(null, "2", "2", "44444", 512523415, 53152341, Solved.DNF)
            )
        )
    }

    fun createEvent(userId: String, puzzleType: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/events/post")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(
                        Event(ObjectId(), userId, puzzleType)

    fun register(username: String, password: String, roles: Set<String> = setOf("USER")) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/api/auth/signup")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(SignupRequest(username, "email@email.com", roles, password)
                    ).toString()
                )

        )

    fun createSession(userId: String, eventId: String, name: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/sessions/post")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(
                        SessionRequest(userId, eventId, name)
                    ).toString()
                )

        )
        
    fun createSessionHex(userId: String, eventId: String, name: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/sessions/hex/post")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(
                        Session(ObjectId("60ce14080000000000000000"), userId, eventId, name)

    fun login(username: String, password: String): ResultActions =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/api/auth/signin")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(
                        LoginRequest(username, password)
                    ).toString()
                )

        )

    fun patchSession(updateName: String = "updateName", id : String = "60ce14080000000000000000") =
        mockMvc.perform(
            MockMvcRequestBuilders.patch("/sessions/patch/$id")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(
                        SessionChange(updateName)
                    ).toString()
                )

        )

    fun ResultActions.bearerToken() : AuthControllerIntTest.LoginResponseData {
        val map = Gson().fromJson(this.andReturn().response.contentAsString, Map::class.java)

        return AuthControllerIntTest.LoginResponseData(
            map["tokenType"] as String + " " + map["accessToken"] as String,
            map["roles"] as ArrayList<String>
        )
    }
}