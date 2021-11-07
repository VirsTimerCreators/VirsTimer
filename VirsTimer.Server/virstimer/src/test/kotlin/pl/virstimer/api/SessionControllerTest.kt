package pl.virstimer.api

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.model.Session

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
class SessionControllerTest : TestCommons() {

    @BeforeEach
    fun injections() {
        before_each()
        mongoTemplate.insert(Session(null, "user-1", "event-1","session_name1"))
        mongoTemplate.insert(Session(null, "user-2", "event-2","session_name2"))
    }

    @Test
    fun should_return_sessions() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        mockMvc.perform(
            MockMvcRequestBuilders.get("/sessions/all").authorizedWith(loginDetails.authHeader)
        )
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)
    }

    @Test
    fun should_return_sessions_for_user() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        mockMvc.perform(
            MockMvcRequestBuilders.get("/sessions/user").authorizedWith(loginDetails.authHeader)
        )
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)
    }

    @Test
    fun should_return_sessions_for_event() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        mockMvc.perform(
            MockMvcRequestBuilders.get("/sessions/event/event-1").authorizedWith(loginDetails.authHeader)
        )
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

        mockMvc.perform(
            MockMvcRequestBuilders.get("/sessions/event/event-2").authorizedWith(loginDetails.authHeader)
        )
            .andExpect(MockMvcResultMatchers.jsonPath("$").isEmpty)

    }

}