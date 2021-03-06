﻿using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Paramore.Brighter.CommandStore.DynamoDB
{
    public class DynamoDbCommandStoreBuilder
    {
        private readonly string _tableName = "brighter_command_store";

        public DynamoDbCommandStoreBuilder() { }

        public DynamoDbCommandStoreBuilder(string tableName)
        {
            _tableName = tableName;
        }

        public CreateTableRequest CreateCommandStoreTableRequest(int readCapacityUnits = 2, int writeCapacityUnits = 1)
        {
            var provisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = readCapacityUnits,
                WriteCapacityUnits = writeCapacityUnits
            };

            return new CreateTableRequest
            {
                TableName = _tableName,
                ProvisionedThroughput = provisionedThroughput,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Command+Date",
                        AttributeType = ScalarAttributeType.S
                    }
                    ,
                    new AttributeDefinition
                    {
                        AttributeName = "Time",
                        AttributeType = ScalarAttributeType.S
                    }
                    ,
                    new AttributeDefinition
                    {
                        AttributeName = "CommandId",
                        AttributeType = ScalarAttributeType.S
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "ContextKey",
                        AttributeType = ScalarAttributeType.S
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Command+Date",
                        KeyType = KeyType.HASH
                    }
                    ,
                    new KeySchemaElement
                    {
                        AttributeName = "Time",
                        KeyType = KeyType.RANGE
                    }
                }
                ,
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                    new GlobalSecondaryIndex
                    {
                        IndexName = "CommandId",
                        ProvisionedThroughput = provisionedThroughput,
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = "CommandId",
                                KeyType = KeyType.HASH
                            }
                        },
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL
                        }
                    },
                    new GlobalSecondaryIndex
                    {
                        IndexName = "ContextKey",
                        ProvisionedThroughput = provisionedThroughput,
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = "ContextKey",
                                KeyType = KeyType.HASH
                            }
                        },
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL
                        }
                    }
                }
            };
        }
    }
}
