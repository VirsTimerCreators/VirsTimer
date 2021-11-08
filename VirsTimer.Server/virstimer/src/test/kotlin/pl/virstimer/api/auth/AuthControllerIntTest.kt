package pl.virstimer.api.auth

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.TestInstance
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.ResultActions
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath
import pl.virstimer.TestCommons

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class AuthControllerIntTest : TestCommons() {

    @BeforeEach
    fun commons() { before_each() }

    @Test
    fun should_allow_creating_account_and_logging_in() {
        register("username", "password", setOf("USER"))
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
}