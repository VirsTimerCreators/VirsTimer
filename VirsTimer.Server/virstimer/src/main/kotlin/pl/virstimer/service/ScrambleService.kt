package pl.virstimer.service

import org.springframework.stereotype.Component
import org.worldcubeassociation.tnoodle.puzzle.*
import pl.virstimer.domain.DomainScramble
import pl.virstimer.domain.PuzzleType
import java.util.*

@Component
class ScrambleService {

    private val random = Random(RANDOM_SEED)

    fun generateScrambleAndSvg(puzzleType: PuzzleType): DomainScramble =
        when (puzzleType) {
            PuzzleType.TWO_BY_TWO -> scrambleTwoByTwo()
            PuzzleType.THREE_BY_THREE -> scrambleThreeByThree()
            PuzzleType.FOUR_BY_FOUR -> scrambleFourByFour()
            PuzzleType.FIVE_BY_FIVE -> scrambleFiveByFive()
            PuzzleType.SIX_BY_SIX -> scrambleSixBySix()
            PuzzleType.SEVEN_BY_SEVEN -> scrambleSevenBySeven()
            PuzzleType.CLOCK -> scrambleClock()
            PuzzleType.MEGAMINX -> scrambleMegaminx()
            PuzzleType.PYRAMINX -> scramblePyraminx()
            PuzzleType.SKWEB -> scrambleSkweb()
            PuzzleType.SQUARE_ONE -> scrambleSquareOne()
        }


    private fun scrambleTwoByTwo(): DomainScramble {
        val puzzle = TwoByTwoCubePuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.TWO_BY_TWO
        )
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

    private fun scrambleFourByFour(): DomainScramble {
        val puzzle = FourByFourCubePuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.FOUR_BY_FOUR
        )
    }

    private fun scrambleFiveByFive(): DomainScramble {
        val puzzle = NoInspectionFiveByFiveCubePuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.FIVE_BY_FIVE
        )
    }

    private fun scrambleSixBySix(): DomainScramble {
        val puzzle = CubePuzzle(6)
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.SIX_BY_SIX
        )
    }

    private fun scrambleSevenBySeven(): DomainScramble {
        val puzzle = CubePuzzle(7)
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.SEVEN_BY_SEVEN
        )
    }

    private fun scrambleClock(): DomainScramble {
        val puzzle = ClockPuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.CLOCK
        )
    }

    private fun scrambleMegaminx(): DomainScramble {
        val puzzle = MegaminxPuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.MEGAMINX
        )
    }

    private fun scramblePyraminx(): DomainScramble {
        val puzzle = PyraminxPuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.PYRAMINX
        )
    }

    private fun scrambleSkweb(): DomainScramble {
        val puzzle = SkewbPuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.SKWEB
        )
    }

    private fun scrambleSquareOne(): DomainScramble {
        val puzzle = SquareOnePuzzle()
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            PuzzleType.SQUARE_ONE
        )
    }

    companion object {
        val RANDOM_SEED: Long = 3434343434 // Move to RandomProvider spring bean
    }
}

