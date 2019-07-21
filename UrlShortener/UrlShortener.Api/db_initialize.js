use url_shortener

db.urls.insert({ _id: 0, sequence_value: 1 })

function getNextSequenceValue() {

    var sequenceDocument = db.urls.findAndModify({
        query: { _id: 0 },
        update: { $inc: { sequence_value: 1 } },
        new: true
    });

    return sequenceDocument.sequence_value;
}

db.urls.insert({ "_id": getNextSequenceValue(), "user": "00001", "url": "test.com/test", "views": 0 })
