#!/bin/bash

models=(
    "address"
    "customer"
    "item"
    "order"
    "robot"
    "role"
    "route"
    "store"
)

for model in "${models[@]}"; do
    for count in {1..2}; do
        file="${model}${count}.json"

        curl -X 'POST' \
        "http://localhost:57679/api/v1/$model" \
        -H 'accept: text/plain' \
        -H 'Content-Type: application/json-patch+json' \
        -d "@$model/$file"

        ## remove comment for debugging the curl command
        # echo curl -X 'POST' \
        # "http://localhost:57679/api/v1/$model" \
        # -H 'accept: text/plain' \
        # -H 'Content-Type: application/json-patch+json' \
        # -d "@$model/$file"
    done
done

