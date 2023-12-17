const express = require('express');
const mongoose = require('mongoose');
const router = require('./routes');
require('dotenv').config();

const app = express();
app.use(express.json());

app.listen(process.env.PORT, () => {
    console.log('server is up and running')
})

app.use(router);

mongoose.connect(process.env.CONNECTIONSTRING, {
    useNewUrlParser: true,
    useUnifiedTopology: true
})