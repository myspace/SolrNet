﻿#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Configuration;
using Autofac;
using AutofacContrib.SolrNet.Config;
using MbUnit.Framework;
using SolrNet;
using SolrNet.Impl;

namespace AutofacContrib.SolrNet.Tests
{
    [TestFixture]
    public class AutofacMulticoreTests
    {
        [Test]
        public void ResolveSolrOperations()
        {
            // Arrange 
            var builder = new ContainerBuilder();
            var cores = new SolrServers {
                                new SolrServerElement {
                                        Id = "entity1",
                                        DocumentType = typeof (Entity1).AssemblyQualifiedName,
                                        Url = "http://localhost:8983/solr/coreEntity1",
                                    },
                            };

            builder.RegisterModule(new SolrNetModule(cores));
            var container = builder.Build();

            // Act
            var m = container.Resolve<ISolrOperations<Entity1>>();

            // Assert
            Assert.IsTrue(m is SolrServer<Entity1>);
        }

        [Test]
        public void ResolveSolrReadOnlyOperations()
        {
            // Arrange 
            var builder = new ContainerBuilder();
            var cores = new SolrServers {
                                new SolrServerElement {
                                        Id = "entity1",
                                        DocumentType = typeof (Entity1).AssemblyQualifiedName,
                                        Url = "http://localhost:8983/solr/coreEntity1",
                                    },
                            };

            builder.RegisterModule(new SolrNetModule(cores));
            var container = builder.Build();

            // Act
            var solrReadOnlyOperations = container.Resolve<ISolrReadOnlyOperations<Entity1>>();

            // Assert
            Assert.IsTrue(solrReadOnlyOperations is SolrServer<Entity1>);
        }

        [Test]
        public void ResolveSolrOperations_withMultiCore()
        {
            // Arrange 
            var builder = new ContainerBuilder();
            var cores = new SolrServers {
                                new SolrServerElement {
                                        Id = "entity1",
                                        DocumentType = typeof (Entity1).AssemblyQualifiedName,
                                        Url = "http://localhost:8983/solr/coreEntity1",
                                    },
                               new SolrServerElement {
                                        Id = "entity2",
                                        DocumentType = typeof (Entity2).AssemblyQualifiedName,
                                        Url = "http://localhost:8983/solr/coreEntity2",
                                },
                            };

            builder.RegisterModule(new SolrNetModule(cores));
            var container = builder.Build();

            // Act
            var solrOperations1 = container.Resolve<ISolrOperations<Entity1>>();
            var solrOperations2 = container.Resolve<ISolrOperations<Entity2>>();

            // Assert
            Assert.IsTrue(solrOperations1 is SolrServer<Entity1>);
            Assert.IsTrue(solrOperations2 is SolrServer<Entity2>);
        }


        [Test]
        public void ResolveSolrOperations_withNamedMultiCore_sameDocumentType() 
        {
            var readCoreId = "entity1_readcore";
            var writeCoreId = "entity1_writecore";

            // Arrange 
            var builder = new ContainerBuilder();
            var cores = new SolrServers {
                                new SolrServerElement {
                                        Id = readCoreId,
                                        DocumentType = typeof (Entity1).AssemblyQualifiedName,
                                        Url = "http://localhost:8983/solr/readcoreEntity1",
                                    },
                               new SolrServerElement {
                                        Id = writeCoreId,
                                        DocumentType = typeof (Entity1).AssemblyQualifiedName,
                                        Url = "http://localhost:8983/solr/writecoreEntity1",
                                },
                            };

            builder.RegisterModule(new SolrNetModule(cores));
            var container = builder.Build();

            // Act
            var readSolrOperations = container.ResolveNamed<ISolrOperations<Entity1>>(readCoreId);
            var writeSolrOperations = container.ResolveNamed<ISolrOperations<Entity1>>(writeCoreId);
            var solrReadOnlyOperations = container.ResolveNamed<ISolrReadOnlyOperations<Entity1>>(readCoreId);

            // Assert
            Assert.IsTrue(readSolrOperations is SolrServer<Entity1>);
            Assert.IsTrue(writeSolrOperations is SolrServer<Entity1>);
            Assert.IsTrue(solrReadOnlyOperations is SolrServer<Entity1>);
        }

        [Test]
        public void ResolveSolrOperations_fromConfigSection()
        {
            // Arrange 
            var builder = new ContainerBuilder();

            var solrConfig = (SolrConfigurationSection)ConfigurationManager.GetSection("solr");
            builder.RegisterModule(new SolrNetModule(solrConfig.SolrServers));

            var container = builder.Build();

            // Act
            var m = container.Resolve<ISolrOperations<Entity1>>();

            // Assert
            Assert.IsTrue(m is SolrServer<Entity1>);
        }
    }

    public class Entity1 { }
    public class Entity2 { }
}