package pl.virstimer.api

import org.junit.jupiter.api.Test

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
class EventControllerTest :TestCommons() {


    @BeforeEach
    fun injections(){ before_each() }


    @Test
    fun posting_event_ok()
    {
        createEvent("1", "FIVE_BY_FIVE ").andExpect(MockMvcResultMatchers.status().isCreated)
    }

}