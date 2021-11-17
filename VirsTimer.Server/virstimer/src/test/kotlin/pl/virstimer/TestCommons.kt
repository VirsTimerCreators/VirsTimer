package pl.virstimer

import com.fasterxml.jackson.databind.ObjectMapper
import com.google.gson.Gson
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.http.MediaType
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.ResultActions
import org.springframework.test.web.servlet.request.MockHttpServletRequestBuilder
import pl.virstimer.api.EventRequest
import pl.virstimer.api.SessionChange
import pl.virstimer.api.SessionRequest
import pl.virstimer.api.SolveRequest
import pl.virstimer.db.security.model.ERole
import pl.virstimer.db.security.model.Role
import pl.virstimer.db.security.model.User
import pl.virstimer.model.*
import pl.virstimer.security.LoginRequest
import pl.virstimer.security.SignupRequest
import kotlin.collections.ArrayList

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
        mongoTemplate.dropCollection(User::class.java)
        mongoTemplate.dropCollection(Role::class.java)

        mongoTemplate.insertAll(
            listOf(
                Role("ROLE_USER", ERole.ROLE_USER),
                Role("ROLE_ADMIN", ERole.ROLE_ADMIN),
                Role("ROLE_MODERATOR", ERole.ROLE_MODERATOR),
            )
        )
        mongoTemplate.insertAll(
            listOf(
                Event("id-1", "1", "THREE_BY_THREE"),
                Event("id-2", "2", "FOUR_BY_FOUR")
            )
        )
    }

    fun registerAndLogin(username: String, password: String, roles: Set<String> = setOf("USER")): LoginResponseData {
        register(username, password, roles)

        return login(username, password).bearerToken()
    }

    fun createEvent(puzzleType: String, token: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/event")
                .contentType(MediaType.APPLICATION_JSON)
                .content(Gson().toJson(EventRequest(puzzleType)))
                .authorizedWith(token)
        )

    fun patchEvent(updatePuzzleType: String = "updatePuzzleType", token: String, eventId: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.patch("/event/$eventId")
                .contentType(MediaType.APPLICATION_JSON)
                .content(Gson().toJson(EventRequest(updatePuzzleType)).toString())
                .authorizedWith(token)
        )

    fun register(username: String, password: String, roles: Set<String> = setOf("USER")) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/api/auth/signup")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(
                        SignupRequest(username, username + "@gmail.com", roles, password)
                    ).toString()
                )
        )

    fun createSession(eventId: String, name: String, token: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/session")
                .contentType(MediaType.APPLICATION_JSON)
                .content(Gson().toJson(SessionRequest(eventId, name)).toString())
                .header("Authorization", token)
        )

    fun patchSession(updateName: String = "updateName", token: String, sessionId: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.patch("/session/$sessionId")
                .contentType(MediaType.APPLICATION_JSON)
                .content(Gson().toJson(SessionChange(updateName)).toString())
                .header("Authorization", token)

        )

    fun createSolve(sessionId: String, solved: Solved, token: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/solve")
                .contentType(MediaType.APPLICATION_JSON)
                .content(Gson().toJson(SolveRequest(sessionId, "", 10, 10, solved)).toString())
                .header("Authorization", token)
        )

    fun patchSolve(solveId: String, newSolved: Solved, token: String) =
        mockMvc.perform(
            MockMvcRequestBuilders.patch("/solve/$solveId")
                .contentType(MediaType.APPLICATION_JSON)
                .content(Gson().toJson(SolveChange(newSolved)).toString())
                .header("Authorization", token)
        )

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

    fun patchSession(updateName: String = "updateName", id: String = "60ce14080000000000000000") =
        mockMvc.perform(
            MockMvcRequestBuilders.patch("/session/patch/$id")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(
                        SessionChange(updateName)
                    ).toString()
                )

        )

    fun ResultActions.bearerToken(): LoginResponseData {
        val map = Gson().fromJson(this.andReturn().response.contentAsString, Map::class.java)

        return LoginResponseData(
            map["tokenType"] as String + " " + map["accessToken"] as String,
            map["roles"] as ArrayList<String>
        )
    }

    fun MockHttpServletRequestBuilder.authorizedWith(token: String) =
        this.header("Authorization", token)
}

data class LoginResponseData(val authHeader: String, val roles: ArrayList<String>)