package pl.virstimer

import com.fasterxml.jackson.databind.ObjectMapper
import com.google.gson.Gson
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.http.MediaType
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.ResultActions
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
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

    fun register(username: String, password: String, roles: Set<String> = setOf("USER")) =
        mockMvc.perform(
            MockMvcRequestBuilders.post("/api/auth/signup")
                .contentType(MediaType.APPLICATION_JSON)
                .content(
                    Gson().toJson(SignupRequest(username, "email@email.com", roles, password)
                    ).toString()
                )

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

    fun ResultActions.bearerToken() : AuthControllerIntTest.LoginResponseData {
        val map = Gson().fromJson(this.andReturn().response.contentAsString, Map::class.java)

        return AuthControllerIntTest.LoginResponseData(
            map["tokenType"] as String + " " + map["accessToken"] as String,
            map["roles"] as ArrayList<String>
        )
    }
}