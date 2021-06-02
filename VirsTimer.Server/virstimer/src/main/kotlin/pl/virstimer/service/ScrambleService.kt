package pl.virstimer.service

import org.springframework.stereotype.Component
import org.worldcubeassociation.tnoodle.puzzle.*
import org.worldcubeassociation.tnoodle.scrambles.Puzzle
import pl.virstimer.domain.DomainScramble
import pl.virstimer.domain.PuzzleType
import java.util.*

@Component
class ScrambleService {

    private val random = Random(RANDOM_SEED)

    fun generateScrambleAndSvg(puzzleType: PuzzleType): DomainScramble =
        when (puzzleType) {
            PuzzleType.TWO_BY_TWO -> scramble(TwoByTwoCubePuzzle(), puzzleType)
            PuzzleType.THREE_BY_THREE -> scramble(ThreeByThreeCubePuzzle(), puzzleType)
            PuzzleType.FOUR_BY_FOUR -> scramble(FourByFourCubePuzzle(), puzzleType)
            PuzzleType.FIVE_BY_FIVE -> scramble(NoInspectionFiveByFiveCubePuzzle(), puzzleType)
            PuzzleType.SIX_BY_SIX -> scramble(CubePuzzle(6), puzzleType)
            PuzzleType.SEVEN_BY_SEVEN -> scramble(CubePuzzle(7), puzzleType)
            PuzzleType.THREE_BY_THREE_BLINDFOLDED -> scramble(ThreeByThreeCubePuzzle(), puzzleType)
            PuzzleType.THREE_BY_THREE_OH -> scramble(ThreeByThreeCubePuzzle(), puzzleType)
            PuzzleType.CLOCK -> scramble(ClockPuzzle(), puzzleType)
            PuzzleType.MEGAMINX -> scramble(MegaminxPuzzle(), puzzleType)
            PuzzleType.PYRAMINX -> scramble(PyraminxPuzzle(), puzzleType)
            PuzzleType.SKWEB -> scramble(SkewbPuzzle(), puzzleType)
            PuzzleType.SQUARE_ONE -> scramble(SquareOnePuzzle(), puzzleType)
            PuzzleType.FOUR_BY_FOUR_BLINDFOLDED -> scramble(FourByFourCubePuzzle(), puzzleType)
            PuzzleType.FIVE_BY_FIVE_BLINDFOLDED -> scramble(NoInspectionFiveByFiveCubePuzzle(), puzzleType)
        }

    private fun scramble(puzzle: Puzzle, puzzleType: PuzzleType): DomainScramble {
        val wcaScramble = puzzle.generateWcaScramble(random)

        return DomainScramble(
            wcaScramble,
            puzzle.drawScramble(wcaScramble, puzzle.defaultColorScheme),
            puzzleType
        )
    }

    companion object {
        val RANDOM_SEED: Long = 3434343434 // Move to RandomProvider spring bean
    }
}

