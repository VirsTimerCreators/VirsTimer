package pl.virstimer.api

import org.bson.types.ObjectId
import org.junit.jupiter.api.Test

import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.domain.PuzzleType
import pl.virstimer.model.*
import pl.virstimer.TestHelper
@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
internal class EventControllerTest:TestHelper() {


    @BeforeEach
    fun injections(){ before_all() }


    @Test
    fun posting_event_ok()
    {
        createEvent("1", "FIVE_BY_FIVE ").andExpect(MockMvcResultMatchers.status().isCreated)
    }




    fun getRepository() {
    }
}