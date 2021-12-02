package pl.virstimer.api

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.test.annotation.DirtiesContext
import org.springframework.test.context.ActiveProfiles
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.model.Session

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
@ActiveProfiles("test")
@DirtiesContext(classMode = DirtiesContext.ClassMode.AFTER_EACH_TEST_METHOD)
class SessionControllerTest : TestCommons() {

    @BeforeEach
    fun injections() {
        before_each()
        mongoTemplate.insert(Session("id-1", "user-1", "event-1","session_name1"))
        mongoTemplate.insert(Session("id-2", "user-2", "event-2","session_name2"))
    }

    @Test
    fun should_return_sessions() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        mockMvc.perform(
            MockMvcRequestBuilders.get("/session").authorizedWith(loginDetails.authHeader)
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
            MockMvcRequestBuilders.get("/session").authorizedWith(loginDetails.authHeader)
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
            MockMvcRequestBuilders.get("/session/event/event-1").authorizedWith(loginDetails.authHeader)
        )
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

        mockMvc.perform(
            MockMvcRequestBuilders.get("/session/event/event-2").authorizedWith(loginDetails.authHeader)
        )
            .andExpect(MockMvcResultMatchers.jsonPath("$").isEmpty)

    }

    @Test
    fun should_patch_session() {
        val loginDetails = registerAndLogin("user-id", "user-1-pass")

        createSession("user-id", "1", "before", loginDetails.authHeader).andExpect(MockMvcResultMatchers.status().isCreated)
        val session = mongoTemplate.find(Query(Criteria.where("userId").`is`("user-id")), Session::class.java).first()
        assert(session.name == "before")

        patchSession("updatePls", loginDetails.authHeader, session.id).andExpect(MockMvcResultMatchers.status().isOk)
        assert(mongoTemplate.find(Query(Criteria.where("userId").`is`("user-id")), Session::class.java).first().name == "updatePls")
    }

    @Test
    fun should_not_patch_session_if_user_is_not_logged_in() {
        createSession("1", "1", "before", "non-existing-token").andExpect(MockMvcResultMatchers.status().is4xxClientError)
        patchSession("updatePls", "non-existing-id").andExpect(MockMvcResultMatchers.status().is4xxClientError)
        //TODO it does work but how can check id $.name is equal to "updatePls"
    }
}