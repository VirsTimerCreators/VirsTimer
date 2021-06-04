package pl.virstimer.api

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestHelper

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
internal class SessionControllerTest : TestHelper() {


    @BeforeEach
    fun injections() {
        before_all()
    }


    @Test
    fun should_return_sessions() {
        //mongoTemplate.insert(Session(ObjectId(),"1", "1","session_name1"))
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/all"))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_return_sessions_for_user() {
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/user/1"))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_return_sessions_for_event() {
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/event/1"))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }
    @Test
    fun should_return_session() {
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/hex/60ce14080000000000000000"))
            .andExpect(MockMvcResultMatchers.jsonPath("$.id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$.userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$.eventId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$.name").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }
    @Test
    fun patching_session_ok() {
        createSession("1", "1", "before").andExpect(MockMvcResultMatchers.status().isCreated)
        patchSession("updatePls").andExpect(MockMvcResultMatchers.status().isOk)
        //TODO it does work but how can check id $.name is equal to "updatePls"
    }


}