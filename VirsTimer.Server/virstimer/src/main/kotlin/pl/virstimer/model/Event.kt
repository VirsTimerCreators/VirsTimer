package pl.virstimer.model


import org.bson.types.ObjectId

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document


// { "_id": "ObjectId", "userId": "String", "puzzleType": "String" }

@Document("events")
class Event(
    @Id var id: ObjectId?=null,
    var userId: String,
    var puzzleType: String
)