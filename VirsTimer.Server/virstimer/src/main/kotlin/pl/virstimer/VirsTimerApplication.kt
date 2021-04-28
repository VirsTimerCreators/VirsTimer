package pl.virstimer

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication

@SpringBootApplication
class VirsTimerApplication

fun main(args: Array<String>) {
	runApplication<VirsTimerApplication>(*args)
}
