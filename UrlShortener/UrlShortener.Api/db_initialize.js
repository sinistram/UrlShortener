use url_shortener

db.urls.insert({ sequence_name: "id", sequence_value: 0 })

function getNextSequenceValue() {

    var sequenceDocument = db.urls.findAndModify({
        query: { sequence_name: "id" },
        update: { $inc: { sequence_value: 1 } },
        new: true
    });

    return sequenceDocument.sequence_value;
}

db.urls.insert({ "_id": getNextSequenceValue(), "user": "00001", "url": "test.com/test", "views": 0 })
