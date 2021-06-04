package pl.virstimer.api

import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.Test

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.db.security.model.User

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
class EventControllerTest :TestCommons() {


    @BeforeEach
    fun injections(){
        beforeEach()
        registerAndLogin()
    }

    @AfterEach
    fun after() {
        mongoTemplate.dropCollection(User::class.java)
    }

    @Test
    fun should_allow_to_post_event_as_logged_user()
    {
        createEvent("1", "FIVE_BY_FIVE ", token).andExpect(MockMvcResultMatchers.status().isCreated)
    }

    @Test
    fun should_not_allow_posting_event_if_not_logged_in() {
        createEvent("1", "FIVE_BY_FIVE ", "not-existing-token").andExpect(MockMvcResultMatchers.status().is4xxClientError)
    }

}