package pl.virstimer.service

import org.springframework.stereotype.Component
import org.worldcubeassociation.tnoodle.puzzle.ThreeByThreeCubePuzzle
import pl.virstimer.domain.DomainScramble
import pl.virstimer.domain.PuzzleType
import java.util.*

@Component
class ScrambleService {

    private val random = Random(RANDOM_SEED)

    fun generateScrambleAndSvg(puzzleType: PuzzleType): DomainScramble =
        when(puzzleType) {
            PuzzleType.THREE_BY_THREE -> scrambleThreeByThree()
            // TODO: Add rest of the competition types
        }


    private fun scrambleThreeByThree(): DomainScramble {
        val puzzle = ThreeByThreeCubePuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
                wcaScramble,
                puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
                PuzzleType.THREE_BY_THREE
        )
    }

    companion object {
        val RANDOM_SEED: Long = 3434343434 // Move to RandomProvider spring bean
    }
}

