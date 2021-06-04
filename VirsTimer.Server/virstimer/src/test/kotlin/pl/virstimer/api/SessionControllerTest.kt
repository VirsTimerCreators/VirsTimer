package pl.virstimer.api

import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.db.security.model.User

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
    fun should_return_sessions() {
        //mongoTemplate.insert(Session(ObjectId(),"1", "1","session_name1"))
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/all").header("Authorization", token))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_return_sessions_for_user() {
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/user/1").header("Authorization", token))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_return_sessions_for_event() {
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/event/1").header("Authorization", token))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_patch_session() {

        createSessionHex("1", "1", "before", token).andExpect(MockMvcResultMatchers.status().isCreated)
        patchSession("updatePls", token).andExpect(MockMvcResultMatchers.status().isOk)
        //TODO it does work but how can check id $.name is equal to "updatePls"
    }

    @Test
    fun should_not_patch_session_if_user_is_not_logged_in() {
        createSessionHex("1", "1", "before", "non-existing-token").andExpect(MockMvcResultMatchers.status().is4xxClientError)
        patchSession("updatePls", "non-existing-token").andExpect(MockMvcResultMatchers.status().is4xxClientError)
        //TODO it does work but how can check id $.name is equal to "updatePls"
    }


}