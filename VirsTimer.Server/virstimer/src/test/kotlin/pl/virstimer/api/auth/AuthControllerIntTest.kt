package pl.virstimer.api.auth

import com.fasterxml.jackson.databind.ObjectMapper
import com.google.gson.Gson
import org.junit.jupiter.api.BeforeAll
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.TestInstance
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.http.MediaType
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.ResultActions
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath
import pl.virstimer.TestCommons
import pl.virstimer.db.security.model.ERole
import pl.virstimer.db.security.model.Role
import pl.virstimer.db.security.model.User
import pl.virstimer.security.LoginRequest
import pl.virstimer.security.SignupRequest

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class AuthControllerIntTest : TestCommons() {

    @BeforeEach
    fun before_all() {
        mongoTemplate.dropCollection(User::class.java)
        mongoTemplate.dropCollection(Role::class.java)
        mongoTemplate.insertAll(
            listOf(
                Role("ROLE_USER", ERole.ROLE_USER),
                Role("ROLE_ADMIN", ERole.ROLE_ADMIN),
                Role("ROLE_MODERATOR", ERole.ROLE_MODERATOR),
            )
        )
    }

    @Test
    fun should_allow_creating_account_and_logging_in() {
        register("username", "password")
            .andExpect(MockMvcResultMatchers.status().isOk)

        login("username", "password")
            .andExpect(MockMvcResultMatchers.status().isOk)
            .andExpect(jsonPath("$.accessToken").isNotEmpty)
            .andExpect(jsonPath("$.tokenType").isNotEmpty)
    }

    @Test
    fun should_allow_to_access_resource_if_user_has_role() {
        register("username", "password", setOf("USER"))
            .andExpect(MockMvcResultMatchers.status().isOk)

        val loginData = login("username", "password")
            .andExpect(MockMvcResultMatchers.status().isOk)
            .bearerToken()

        accessResource("all", loginData.authHeader)
            .andExpect(MockMvcResultMatchers.status().isOk)

        accessResource("user", loginData.authHeader)
            .andExpect(MockMvcResultMatchers.status().isOk)

        accessResource("mod", loginData.authHeader)
            .andExpect(MockMvcResultMatchers.status().is4xxClientError)

        accessResource("admin", loginData.authHeader)
            .andExpect(MockMvcResultMatchers.status().is4xxClientError)
    }


    private fun accessResource(resource: String, authHeader: String): ResultActions =
        mockMvc.perform(get("/api/test/$resource")
            .header("Authorization", authHeader))


    data class LoginResponseData(val authHeader: String, val roles: ArrayList<String>)
}