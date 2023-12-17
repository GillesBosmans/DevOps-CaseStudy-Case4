const mongoose = require('mongoose');

const UserLoginSchema = new mongoose.Schema({
    username: {
        type: String
    },
    password: {
        type: String
    },
    role: {
        type: String
    }
});

module.exports = mongoose.model('logins', UserLoginSchema);