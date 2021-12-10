package pl.virstimer.api.multiplayer

import org.hamcrest.Matcher
import org.hamcrest.core.Is.`is`
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.data.mongodb.core.findAll
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.test.context.ActiveProfiles
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.domain.PuzzleType
import pl.virstimer.model.multiplayer.PersistentRoom
import pl.virstimer.model.multiplayer.PersistentScramble

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
@ActiveProfiles("test")
class RoomControllerTest : TestCommons() {

    @BeforeEach
    fun beforeEach() { before_each() }

    @Test
    fun `should create room`() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        createRoom(2, PuzzleType.THREE_BY_THREE, loginDetails.authHeader)

        val result = mongoTemplate.findAll(PersistentRoom::class.java)

        assert(result.size == 1)

        with(result.first()) {
            assert(this.status == RoomStatus.OPEN)
            assert(this.administratorId == "user-1")
            assert(this.users == setOf("user-1"))
            assert(this.scrambleIds.size == 2)
        }
    }

    @Test
    fun `creating room should create scrambles`() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        createRoom(3, PuzzleType.THREE_BY_THREE, loginDetails.authHeader)

        val scrambleIds = mongoTemplate.findAll(PersistentRoom::class.java).first().scrambleIds

        assert(scrambleIds.size == 3)

        val scrambles = mongoTemplate.findAll(PersistentScramble::class.java)

        assert(scrambles.size == 3)

        assert(scrambles
            .filter { !it.scrambleText.isEmpty()}
            .filter { it.puzzleType == PuzzleType.THREE_BY_THREE }
            .size == 3)
    }

    @Test
    fun `should only allow administrator to change status`() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")
        val otherUserLoginDetails = registerAndLogin("other-user", "other-user-pass")

        createRoom(3, PuzzleType.THREE_BY_THREE, loginDetails.authHeader)

        var room = mongoTemplate.findAll(PersistentRoom::class.java).first()

        assert(room.status == RoomStatus.OPEN)

        modifyRoomStatus(room.id, RoomStatus.INPROGRESS, loginDetails.authHeader)

        room = mongoTemplate.findAll(PersistentRoom::class.java).first()

        assert(room.status == RoomStatus.INPROGRESS)

        modifyRoomStatus(room.id, RoomStatus.OPEN, otherUserLoginDetails.authHeader)

        assert(room.status == RoomStatus.INPROGRESS)
    }

    @Test
    fun `should not allow to change status after it was set to CLOSED status`() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        createRoom(3, PuzzleType.THREE_BY_THREE, loginDetails.authHeader)
        var room = mongoTemplate.findAll(PersistentRoom::class.java).first()

        modifyRoomStatus(room.id, RoomStatus.CLOSED, loginDetails.authHeader)
        room = mongoTemplate.findAll(PersistentRoom::class.java).first()
        assert(room.status == RoomStatus.CLOSED)

        modifyRoomStatus(room.id, RoomStatus.OPEN, loginDetails.authHeader)
        room = mongoTemplate.findAll(PersistentRoom::class.java).first()
        assert(room.status == RoomStatus.CLOSED)

        modifyRoomStatus(room.id, RoomStatus.INPROGRESS, loginDetails.authHeader)
        room = mongoTemplate.findAll(PersistentRoom::class.java).first()
        assert(room.status == RoomStatus.CLOSED)
    }

    @Test
    fun `should allow to join room by code and leave`() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")
        val otherUserLoginDetails = registerAndLogin("user-2", "user-2-pass")
        val anotherUserLoginDetails = registerAndLogin("user-3", "user-3-pass")

        createRoom(3, PuzzleType.THREE_BY_THREE, loginDetails.authHeader)
        var room = mongoTemplate.findAll(PersistentRoom::class.java).first()

        assert(room.users == setOf("user-1"))

        joinRoom(room.joinCode, otherUserLoginDetails.authHeader)
        room = mongoTemplate.findAll(PersistentRoom::class.java).first()
        assert(room.users == setOf("user-1", "user-2"))

        joinRoom(room.joinCode, anotherUserLoginDetails.authHeader)
        room = mongoTemplate.findAll(PersistentRoom::class.java).first()
        assert(room.users == setOf("user-1", "user-2", "user-3"))

        leaveRoom(room.id, otherUserLoginDetails.authHeader)
        room = mongoTemplate.findAll(PersistentRoom::class.java).first()
        assert(room.users == setOf("user-1", "user-3"))
    }

    @Test
    fun `should return scrambles for specific room`() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")
        val someOtherUser = registerAndLogin("user-X", "user-X-pass")

        createRoom(10, PuzzleType.THREE_BY_THREE, someOtherUser.authHeader)
        createRoom(5, PuzzleType.FOUR_BY_FOUR, someOtherUser.authHeader)
        createRoom(3, PuzzleType.THREE_BY_THREE, loginDetails.authHeader)
        var room = mongoTemplate.find(Query(Criteria.where("administratorId").`is`("user-1")), PersistentRoom::class.java).first()

        getRoomScrambles(room.id, loginDetails.authHeader)
            .andExpect(MockMvcResultMatchers.status().isOk)
            .andExpect(MockMvcResultMatchers.jsonPath("$.length()", `is`(3)))
    }

}