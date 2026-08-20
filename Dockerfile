FROM quay.io/debezium/connect:latest

USER root

# تسطيب unzip
RUN microdnf -y install unzip && microdnf clean all

RUN curl -fSL -o /tmp/avro.zip "https://d1i4a15mxbxib1.cloudfront.net/api/plugins/confluentinc/kafka-connect-avro-converter/versions/7.5.0/confluentinc-kafka-connect-avro-converter-7.5.0.zip" \
    && unzip /tmp/avro.zip -d /tmp/ \
    && mkdir -p /kafka/connect/avro \
    && cp /tmp/confluentinc-kafka-connect-avro-converter-7.5.0/lib/* /kafka/connect/avro/ \
    && rm -rf /tmp/avro.zip /tmp/confluentinc-kafka-connect-avro-converter-7.5.0

USER 1001