using System.Linq;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.PresentationAppearanceResource;
using Xbim.Ifc4.PresentationOrganizationResource;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.IO;
using Xbim.Ifc4.MaterialResource;
using Xbim.Ifc4.PropertyResource;



static class TetrahedronExample
    {
        /// <summary>
        /// This sample demonstrates the minimum steps to create a compliant IFC model that contains a single standard case cube
        /// </summary>
        public static void Run(TesselationType tesselationType)
        {
            Console.WriteLine("Initialising the IFC Project....");

            //first we need to set up some credentials for ownership of data in the new model
            var credentials = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "xBimTeam",
                ApplicationFullName = "Cube Example",
                ApplicationIdentifier = "CE",
                ApplicationVersion = "1.0",
                EditorsFamilyName = "Team",
                EditorsGivenName = "xBIM",
                EditorsOrganisationName = "xBimTeam"
            };

            using (var model = IfcStore.Create(credentials, XbimSchemaVersion.Ifc4, XbimStoreType.InMemoryModel))
            {
                using (var txn = model.BeginTransaction("Creating content"))
                {
                    var project = InitializeProject(model, "World");
                    var building2 = CreateBuilding(model, project, "01");
                    var building = CreateBuilding(model, project, "02");
                    var product = CreateProduct(model, building, tesselationType);
                    CreateSimpleProperty(model, (IfcBuildingElementProxy)product);
                
                txn.Commit();
                }



                //write the Ifc File
                model.SaveAs(@"C:/Tetrahedron.ifc", StorageType.Ifc);
            }
            Console.WriteLine("C:/Tetrahedron.ifc file created and saved.");
        }

    private static void CreateSimpleProperty(IfcStore model, IfcBuildingElementProxy wall)
    {
        var Parametr1 = model.Instances.New<IfcPropertySingleValue>(psv =>
        {
            psv.Name = "Параматр1";
            psv.Description = "";
            psv.NominalValue = new IfcLengthMeasure(150.0);
            psv.Unit = model.Instances.New<IfcSIUnit>(siu =>
            {
                siu.UnitType = IfcUnitEnum.LENGTHUNIT;
                siu.Name = IfcSIUnitName.METRE;
            });
        });

        var Parametr2 = model.Instances.New<IfcPropertySingleValue>(psv =>
        {
            psv.Name = "Параматр2";
            psv.Description = "";
            psv.NominalValue = new IfcLengthMeasure(150.0);
            psv.Unit = model.Instances.New<IfcSIUnit>(siu =>
            {
                siu.UnitType = IfcUnitEnum.LENGTHUNIT;
                siu.Name = IfcSIUnitName.METRE;
            });
        });

        var Parametr3 = model.Instances.New<IfcPropertySingleValue>(psv =>
        {
            psv.Name = "Параматр3";
            psv.Description = "";
            psv.NominalValue = new IfcLengthMeasure(150.0);
            psv.Unit = model.Instances.New<IfcSIUnit>(siu =>
            {
                siu.UnitType = IfcUnitEnum.LENGTHUNIT;
                siu.Name = IfcSIUnitName.METRE;
            });
        });
        var Parametr4 = model.Instances.New<IfcPropertySingleValue>(psv =>
        {
            psv.Name = "Параматр4";
            psv.Description = "";
            psv.NominalValue = new IfcLengthMeasure(150.0);
            psv.Unit = model.Instances.New<IfcSIUnit>(siu =>
            {
                siu.UnitType = IfcUnitEnum.LENGTHUNIT;
                siu.Name = IfcSIUnitName.METRE;
            });
        });

        var Parametr5 = model.Instances.New<IfcPropertySingleValue>(psv =>
        {
            psv.Name = "Параматр5";
            psv.Description = "";
            psv.NominalValue = new IfcLengthMeasure(150.0);
            psv.Unit = model.Instances.New<IfcSIUnit>(siu =>
            {
                siu.UnitType = IfcUnitEnum.LENGTHUNIT;
                siu.Name = IfcSIUnitName.METRE;
            });
        });

        var ifcMaterial = model.Instances.New<IfcMaterial>(m =>
        {
            m.Name = "Brick";
        });
        var ifcPrValueMaterial = model.Instances.New<IfcPropertyReferenceValue>(prv =>
        {
            prv.Name = "IfcPropertyReferenceValue:Material";
            prv.PropertyReference = ifcMaterial;
        });


        var ifcMaterialList = model.Instances.New<IfcMaterialList>(ml =>
        {
            ml.Materials.Add(ifcMaterial);
            ml.Materials.Add(model.Instances.New<IfcMaterial>(m => { m.Name = "Cavity"; }));
            ml.Materials.Add(model.Instances.New<IfcMaterial>(m => { m.Name = "Block"; }));
        });


        var ifcMaterialLayer = model.Instances.New<IfcMaterialLayer>(ml =>
        {
            ml.Material = ifcMaterial;
            ml.LayerThickness = 100.0;
        });
        var ifcPrValueMatLayer = model.Instances.New<IfcPropertyReferenceValue>(prv =>
        {
            prv.Name = "IfcPropertyReferenceValue:MaterialLayer";
            prv.PropertyReference = ifcMaterialLayer;
        });


        //lets create the IfcElementQuantity
        var ifcPropertySet = model.Instances.New<IfcPropertySet>(ps =>
        {
            ps.Name = "Test:IfcPropertySet";
            ps.Description = "Property Set";
            ps.HasProperties.Add(Parametr1);
            ps.HasProperties.Add(Parametr2);
            ps.HasProperties.Add(Parametr3);
            ps.HasProperties.Add(Parametr4);
            ps.HasProperties.Add(Parametr5);
            ps.HasProperties.Add(ifcPrValueMaterial);
            ps.HasProperties.Add(ifcPrValueMatLayer);
        });

        //need to create the relationship
        model.Instances.New<IfcRelDefinesByProperties>(rdbp =>
        {
            rdbp.Name = "Property Association";
            rdbp.Description = "IfcPropertySet associated to wall";
            rdbp.RelatedObjects.Add(wall);
            rdbp.RelatingPropertyDefinition = ifcPropertySet;
        });
    }

    private static IfcBuilding CreateBuilding(IfcStore model, IfcProject project, string name)
        {
            var building = model.Instances.New<IfcBuilding>(b => b.Name = name);
            project.AddBuilding(building);
            return building;
        }



        /// <summary>
        /// Sets up the basic parameters any model must provide, units, ownership etc
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <returns></returns>
        private static IfcProject InitializeProject(IModel model, string projectName)
        {
            var i = model.Instances;

            //create a project
            var project = model.Instances.New<IfcProject>(p => p.Name = projectName);

            //set the units, at least length unit and plane angle units are needed for geometry definitions to work
            project.UnitsInContext = i.New<IfcUnitAssignment>(a =>
            {
                a.Units.AddRange(new[] {
                    i.New<IfcSIUnit>(u => {
                        u.Name = IfcSIUnitName.METRE;
                        u.Prefix = IfcSIPrefix.MILLI;
                    }),
                    i.New<IfcSIUnit>(u => {
                        u.UnitType = IfcUnitEnum.PLANEANGLEUNIT;
                        u.Name = IfcSIUnitName.RADIAN;
                    })
                });
            });

            // create model representation context
            project.RepresentationContexts.Add(i.New<IfcGeometricRepresentationContext>(c =>
            {
                c.ContextType = "Model";
                c.ContextIdentifier = "Building Model";
                c.CoordinateSpaceDimension = 3;
                c.Precision = 0.00001;
                c.WorldCoordinateSystem = i.New<IfcAxis2Placement3D>(a => a.Location = i.New<IfcCartesianPoint>(p => p.SetXYZ(0, 0, 0)));
            }
            ));

            // create plan representation context
            project.RepresentationContexts.Add(i.New<IfcGeometricRepresentationContext>(c =>
            {
                c.ContextType = "Plan";
                c.ContextIdentifier = "Building Plan View";
                c.CoordinateSpaceDimension = 2;
                c.Precision = 0.00001;
                c.WorldCoordinateSystem = i.New<IfcAxis2Placement2D>(a => a.Location = i.New<IfcCartesianPoint>(p => p.SetXY(0, 0)));
            }
            ));

            //now commit the changes, else they will be rolled back at the end of the scope of the using statement
            return project;

        }

        /// <summary>
        /// This creates a product and it's geometry, many geometric representations are possible. 
        /// This example uses triangulated face set
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Product with placement and geometry representation</returns>
        private static IfcProduct CreateProduct(IModel model, IfcBuilding parent, TesselationType tesselationType)
        {
            var i = model.Instances;

            // geometry as a face set
            IfcRepresentationItem createBody() => tesselationType switch
            {
                TesselationType.TriangulatedFaceSet => CreateTriangulatedFaceSet(model),
                TesselationType.PolygonalFaceSet => CreatePolygonalFaceSet(model),
                _ => throw new System.NotImplementedException()
            };

            var body = createBody();

            // create a Definition shape to hold the geometry
            var shape = i.New<IfcShapeRepresentation>(s => {
                s.ContextOfItems = i.OfType<IfcGeometricRepresentationContext>().First();
                s.RepresentationType = "Tessellation";
                s.RepresentationIdentifier = "Body";
                s.Items.Add(body);
            });

            // IfcPresentationLayerAssignment is required for CAD presentation
            var ifcPresentationLayerAssignment = i.New<IfcPresentationLayerAssignment>(layer => {
                layer.Name = "Furnishing Elements";
                layer.AssignedItems.Add(shape);
            });

            // create visual style
            i.New<IfcStyledItem>(styleItem =>
            {
                styleItem.Item = body;
                styleItem.Styles.Add(i.New<IfcSurfaceStyle>(style =>
                {
                    style.Side = IfcSurfaceSide.BOTH;
                    style.Styles.Add(i.New<IfcSurfaceStyleRendering>(rendering =>
                    {
                        rendering.SurfaceColour = i.New<IfcColourRgb>(colour =>
                        {
                            colour.Name = "Orange";
                            colour.Red = 0.0;
                            colour.Green = 0.0;
                            colour.Blue = 1.0;
                        });
                    }));
                }));
            });

            var proxy = i.New<IfcBuildingElementProxy>(c => {
                c.Name = "The Tetrahedron";

                // create a Product Definition and add the model geometry to the cube
                c.Representation = i.New<IfcProductDefinitionShape>(r => r.Representations.Add(shape));

                // now place the object into the model
                c.ObjectPlacement = i.New<IfcLocalPlacement>(p => p.RelativePlacement = i.New<IfcAxis2Placement3D>(a => {
                    a.Location = i.New<IfcCartesianPoint>(cp => cp.SetXYZ(0, 0, 0));
                    a.RefDirection = i.New<IfcDirection>();
                    a.RefDirection.SetXYZ(0, 1, 0);
                    a.Axis = i.New<IfcDirection>();
                    a.Axis.SetXYZ(0, 0, 1);
                }));
            });


            parent.AddElement(proxy);
            return proxy;
        }

        /// <summary>
        /// Creates the simplest 3D triangulated face set
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Tetrahedron as a triangulated face set</returns>
        private static IfcTriangulatedFaceSet CreateTriangulatedFaceSet(IModel model)
        {
            return model.Instances.New<IfcTriangulatedFaceSet>(tfs => {
                tfs.Closed = true;
                tfs.Coordinates = model.Instances.New<IfcCartesianPointList3D>(pl => {
                    pl.CoordList.GetAt(0).AddRange(new IfcLengthMeasure[] { -1, -1, -2 });
                    pl.CoordList.GetAt(1).AddRange(new IfcLengthMeasure[] { 0, 1, -2 });
                    pl.CoordList.GetAt(2).AddRange(new IfcLengthMeasure[] { 1, -1, -2 });
                });

                // Indices are 1 based in IFC!
                tfs.CoordIndex.GetAt(0).AddRange(new IfcPositiveInteger[] { 1, 2, 3 });


            });
        }

        private static IfcPolygonalFaceSet CreatePolygonalFaceSet(IModel model)
        {
            var polyfaceset = model.Instances.New<IfcPolygonalFaceSet>();
            polyfaceset.Closed = true;

            polyfaceset.Coordinates = model.Instances.New<IfcCartesianPointList3D>(pl => {
                pl.CoordList.GetAt(0).AddRange(new IfcLengthMeasure[] { 0, 0, 0 });
                pl.CoordList.GetAt(1).AddRange(new IfcLengthMeasure[] { 2, 0, -2 });
                pl.CoordList.GetAt(2).AddRange(new IfcLengthMeasure[] { 0, 2, 3 });
            });
            polyfaceset.Faces.AddRange(new[] {
                model.Instances.New<IfcIndexedPolygonalFace>(face => face.CoordIndex.AddRange(new IfcPositiveInteger[] { 1, 2, 3 })),
            });

            return polyfaceset;
        }
    }

    internal enum TesselationType
    {
        TriangulatedFaceSet,
        PolygonalFaceSet
    }
