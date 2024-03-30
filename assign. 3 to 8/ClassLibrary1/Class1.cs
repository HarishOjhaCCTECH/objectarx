using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Colors;

[assembly:CommandClass(typeof(ClassLibrary1.Class1))]
namespace ClassLibrary1
{
    public class Class1
    {
        [CommandMethod("hello")]
        public void helloWord()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ed.WriteMessage("Hello everyon, whoever is watching!");
        }

        [CommandMethod("AddLine")]
        public static void AddLine()
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a line that starts at 5,5 and ends at 12,3
                using (Line acLine = new Line(new Point3d(5, 5, 0),
                                              new Point3d(12, 3, 0)))
                {

                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acLine);
                    acTrans.AddNewlyCreatedDBObject(acLine, true);
                }

                // Save the new object to the database
                acTrans.Commit();
            }
        }

        [CommandMethod("CreateAndAssignALayer")]
        public static void CreateAndAssignALayer()
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Layer table for read
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                                                OpenMode.ForRead) as LayerTable;

                string sLayerName = "Center";

                if (acLyrTbl.Has(sLayerName) == false)
                {
                    using (LayerTableRecord acLyrTblRec = new LayerTableRecord())
                    {
                        // Assign the layer the ACI color 3 and a name
                        acLyrTblRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);
                        acLyrTblRec.Name = sLayerName;

                        // Upgrade the Layer table for write
                        acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite);

                        // Append the new layer to the Layer table and the transaction
                        acLyrTbl.Add(acLyrTblRec);
                        acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);
                    }
                }

                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a circle object
                using (Circle acCirc = new Circle())
                {
                    acCirc.Center = new Point3d(2, 2, 0);
                    acCirc.Radius = 1;
                    acCirc.Layer = sLayerName;

                    acBlkTblRec.AppendEntity(acCirc);
                    acTrans.AddNewlyCreatedDBObject(acCirc, true);
                }

                // Save the changes and dispose of the transaction
                acTrans.Commit();
            }
        }
        [CommandMethod("ListEntities")]
        public void ListEntities()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Open the Block table record for read
                BlockTableRecord btr = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead) as BlockTableRecord;

                // Iterate through each entity in the block table record
                foreach (ObjectId entId in btr)
                {
                    Entity ent = tr.GetObject(entId, OpenMode.ForRead) as Entity;
                    if (ent != null)
                    {
                        // Display the class name of the entity
                        doc.Editor.WriteMessage("\nClass name: " + ent.GetType().Name);
                    }
                }
            }
        }



        [CommandMethod("InputLine")]
        public static void InputLine()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Open the Block table record for write
                BlockTableRecord btr = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                // Prompt for the first point
                PromptPointResult ptResult1 = ed.GetPoint("\nSpecify first point: ");
                if (ptResult1.Status == PromptStatus.OK)
                {
                    Point3d startPt = ptResult1.Value;

                    // Prompt for the second point
                    PromptPointOptions ptOpts2 = new PromptPointOptions("\nSpecify second point: ");
                    ptOpts2.UseBasePoint = true;
                    ptOpts2.BasePoint = startPt;
                    PromptPointResult ptResult2 = ed.GetPoint(ptOpts2);

                    if (ptResult2.Status == PromptStatus.OK)
                    {
                        Point3d endPt = ptResult2.Value;

                        // Create and append the new Line object
                        using (Line line = new Line(startPt, endPt))
                        {
                            btr.AppendEntity(line);
                            tr.AddNewlyCreatedDBObject(line, true);
                        }
                    }
                }

                tr.Commit();
            }
        }

        [CommandMethod("ChangeColor")]
        public static void ChangeColor()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selRes = ed.GetSelection();
            if (selRes.Status == PromptStatus.OK)
            {
                ObjectId[] ids = selRes.Value.GetObjectIds();
                int count = ids.Length;
                ed.WriteMessage("\nObjects selected: " + count);

                string kWord = ed.GetString("\nEnter a color [Red/Yellow/Green/Bylayer] <Red>: ").StringResult;
                if (string.IsNullOrEmpty(kWord))
                    kWord = "Red";

                foreach (ObjectId id in ids)
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        Entity ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {
                            switch (kWord.ToUpper())
                            {
                                case "1":
                                case "RED":
                                    ent.ColorIndex = 1;
                                    break;
                                case "2":
                                case "YELLOW":
                                    ent.ColorIndex = 2;
                                    break;
                                case "3":
                                case "GREEN":
                                    ent.ColorIndex = 3;
                                    break;
                                case "BYLAYER":
                                    ent.ColorIndex = 256; // ByLayer color index
                                    break;
                                default:
                                    ed.WriteMessage("\nInvalid color option. Setting to Red by default.");
                                    ent.ColorIndex = 1;
                                    break;
                            }
                        }
                        tr.Commit();
                    }
                }
            }
        }
        

    }
}



/*
static void listObjects()
{
// Get the current database
AcDbDatabase *pDb = acdbHostApplicationServices()->workingDatabase();
// Get the current space object
AcDbBlockTableRecord *pBlockTableRecord;
Acad::ErrorStatus es = acdbOpenObject(pBlockTableRecord, 
pDb->currentSpaceId(), 
AcDb::kForRead);
// Create a new block iterator that will be used to
// step through each object in the current space
AcDbBlockTableRecordIterator *pItr;
pBlockTableRecord->newIterator(pItr);
// Create a variable AcDbEntity type which is a generic 
// object to represent a Line, Circle, Arc, among other objects
AcDbEntity *pEnt;
// Step through each object in the current space
for (pItr->start(); !pItr->done(); pItr->step())
{
// Get the entity and open it for read
pItr->getEntity(pEnt, AcDb::kForRead);
// Display the class name for the entity before
// closing the object
acutPrintf(_T("\nClass name: %s"), pEnt->isA()->name());
pEnt->close();
}
// Close the current space object
pBlockTableRecord->close();
// Remove the block iterator object from memory
delete pItr;
// Display the AutoCAD Text Window
acedTextScr();
}

*/

