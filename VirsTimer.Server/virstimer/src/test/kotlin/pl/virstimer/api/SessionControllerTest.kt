package pl.virstimer.api

import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.db.security.model.User
import pl.virstimer.model.Session

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
class SessionControllerTest : TestCommons() {

    @BeforeEach
    fun injections() {
        beforeEach()
        registerAndLogin()
    }

    @AfterEach
    fun after() {
        mongoTemplate.dropCollection(User::class.java)
    }

    @Test
    fun should_return_sessions_for_user() {
        mockMvc.perform(MockMvcRequestBuilders.get("/session/user/1").header("Authorization", token))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_return_sessions_for_event_and_user() {
        mockMvc.perform(MockMvcRequestBuilders.get("/session/event/1/user/1").header("Authorization", token))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_patch_session() {
        createSession("user-id", "1", "before", token).andExpect(MockMvcResultMatchers.status().isCreated)
        val session = mongoTemplate.find(Query(Criteria.where("userId").`is`("user-id")), Session::class.java).first()
        assert(session.name == "before")

        patchSession("updatePls", token, session.id).andExpect(MockMvcResultMatchers.status().isOk)
        assert(mongoTemplate.find(Query(Criteria.where("userId").`is`("user-id")), Session::class.java).first().name == "updatePls")
    }

    @Test
    fun should_not_patch_session_if_user_is_not_logged_in() {
        createSession("1", "1", "before", "non-existing-token").andExpect(MockMvcResultMatchers.status().is4xxClientError)
        patchSession("updatePls", "non-existing-token", "11").andExpect(MockMvcResultMatchers.status().is4xxClientError)
        //TODO it does work but how can check id $.name is equal to "updatePls"
    }
}