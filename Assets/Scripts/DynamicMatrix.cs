using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class DynamicMatrix {
	protected int numRows;
	protected int numCols;

	private float[][] elements;

	private bool isValidIndices(int row, int col){
		if(row < 0 || row > numRows-1)  throw new Exception("row(" + row + ") is not valid for " + numRows +"x"+numCols + " matrix");
		if(col < 0 || col > numCols-1)  throw new Exception("col(" + col + ") is not valid for " + numRows +"x"+numCols + " matrix");
		return true;
	}
	public void set(int row, int col, float v) {
		if(!isValidIndices(row,col)) return;
		elements[row][col] = v;
	}
	public void setIncrement(int row, int col, float v) {
		isValidIndices(row,col);
		elements[row][col] += v;
	}
	public float get(int row, int col) {
		isValidIndices(row,col);
		return elements[row][col];
	}
	public int getNumRows() { 	return numRows;  	}
	public int getNumCols() {	return numCols; }

	private void createElement(int rows, int cols){
		numRows = rows;
		numCols = cols;
		elements = new float[numRows][];
		for ( int i = 0; i < numRows; i++ )
			elements [ i ] = new float[numCols];
//		elements = new  float[numRows][numCols];
	}

	public DynamicMatrix(int rows, int cols) {
		createElement(rows, cols);
		for (int row = 0; row < getNumRows(); row++)
			for (int col = 0; col < getNumCols(); col++) {
				set(row, col, 0.0f);
			}
	}

	public DynamicMatrix(DynamicMatrix m) {
		createElement(m.getNumRows(), m.getNumCols());
		for (int row = 0; row < getNumRows(); row++)
			for (int col = 0; col < getNumCols(); col++)
				set(row, col, m.get(row, col));
	}

	public string toString() {
		StringBuilder buf = new StringBuilder();
		buf.Append("             " +getNumRows()+" x " + getNumCols() + " \n");
		int row,col;
		for( col=0; col < getNumCols(); col++){
			buf.Append("        J"+col + "");
		}
		buf.Append("\n");
		for(col = 0; col < getNumCols(); col ++)
			buf.Append("   ------");
		buf.Append("\n");
		for ( row = 0; row < getNumRows(); row++) {
			buf.Append("r" + row + "|   ");
			for ( col = 0; col < getNumCols(); col++) {
				buf.Append(string.Format("%03.3f",get(row,col)));
				buf.Append("     ");
			}
			buf.Append("\n");
		}
		buf.Append("");
		return buf.ToString();
	}

	public DynamicMatrix subtractFrom(DynamicMatrix m) {
		if (getNumRows() != m.getNumRows() || getNumCols() != m.getNumCols()) {
			throw new Exception("dimensions bad in addTo()");
		}
		DynamicMatrix ret = new DynamicMatrix(getNumRows(), getNumCols());

		for (int r = 0; r < getNumRows(); r++)
			for (int c = 0; c < getNumCols(); c++)
				ret.set(r, c, this.get(r, c) - m.get(r, c));

		return ret;
	}
	/**
	 * Calculates the matrix's Moore-Penrose pseudoinverse 
	 * @return an MxN (row x col) matrix which is the matrix's pseudoinverse.
	 */
	public DynamicMatrix pseudoInverse() {

		int r, c;

		int k = 1;
		DynamicMatrix ak = new DynamicMatrix(getNumRows(), 1);
		DynamicMatrix dk, ck, bk;

		DynamicMatrix R_plus;

		for (r = 0; r < getNumRows(); r++)
			ak.set(r,0,  this.get(r,0));

		if ( !ak.EqualsZero () ) {
//		if (!ak.equals(0.0)) {
			R_plus = ak.transpose().multiply(1.0f / (ak.dot(ak)));
		} else {
			R_plus = new DynamicMatrix(1, getNumCols());
		}

		while (k < this.getNumCols()) {

			for (r = 0; r < getNumRows(); r++)
				ak.set(r,0,  this.get(r,k));

			dk = R_plus.multiply(ak);
			DynamicMatrix T = new DynamicMatrix(getNumRows(), k);
			for (r = 0; r < getNumRows(); r++)
				for (c = 0; c < k; c++)
					T.set(r,c, this.get(r,c));

			ck = ak.subtractFrom(T.multiply(dk));

			if ( !ck.EqualsZero () ) {
//			if (!ck.equals(0.0)) {
				bk = ck.transpose().multiply(1.0f / (ck.dot(ck)));
			} else {
				bk = dk.transpose().multiply(1.0f / (1.0f + dk.dot(dk)))
					.multiply(R_plus);
			}

			DynamicMatrix N = R_plus.subtractFrom(dk.multiply(bk));
			R_plus = new DynamicMatrix(N.getNumRows() + 1, N.getNumCols());

			for (r = 0; r < N.getNumRows(); r++)
				for (c = 0; c < N.getNumCols(); c++)
					R_plus.set(r,c,  N.get(r,c));
			for (c = 0; c < N.getNumCols(); c++)
				R_plus.set(R_plus.getNumRows() - 1, c, bk.get(0,c));

			k++;
		}
		return R_plus;
	}
	/**
	 * @return the transposed matrix with dimensions getNumCols() x getNumRows()
	 */
	public DynamicMatrix transpose() {
		DynamicMatrix ret = new DynamicMatrix(getNumCols(), getNumRows());

		for (int r = 0; r < getNumRows(); r++)
			for (int c = 0; c < getNumCols(); c++)
				ret.set(c, r, get(r, c));
		return ret;
	}


	public DynamicMatrix multiply(float s) {
		DynamicMatrix ret = new DynamicMatrix(getNumRows(), getNumCols());
		for (int i = 0; i < getNumRows(); i++)
			for (int j = 0; j < getNumCols(); j++)
				ret.set(i, j, get(i, j) * s);
		return ret;
	}

	public String getDimension(){
		return "["+getNumRows() + "x" + getNumCols()+"]";
	}
	public DynamicMatrix multiply(DynamicMatrix m) {
		if (getNumCols() != m.getNumRows()) {
			throw new Exception(this.getDimension() + " * " + m.getDimension() + " dimensions bad in multiply()");

		}

		DynamicMatrix ret = new DynamicMatrix(getNumRows(), m.getNumCols());

		for (int r = 0; r < getNumRows(); r++)
			for (int c = 0; c < m.getNumCols(); c++) {
				for (int k = 0; k < getNumCols(); k++) {
					ret.set(r, c, ret.get(r, c) + (this.get(r, k) * m.get(k, c)));
				}
			}

		return ret;
	}


	public float dot(DynamicMatrix m) {

		if (getNumRows() != m.getNumRows() || getNumCols() != m.getNumCols()) {
			throw new Exception("dimensions bad in dot()");

		}
		float sum = 0;

		for (int r = 0; r < getNumRows(); r++)
			for (int c = 0; c < getNumCols(); c++)
				sum += this.get(r,c) * m.get(r,c);

		return sum;
	}

	public bool Equals (DynamicMatrix m)
	{
		if ( m == null )
			return false;

		if ( this == m )
			return true;

		if ( numRows != m.numRows || numCols != m.numCols )
			return false;
		
		for ( int i = 0; i < numRows; i++ )
			for ( int j = 0; j < numCols; j++ )
				if ( get ( i, j ) != m.get ( i, j ) )
					return false;
		
		return true;
	}

	public bool EqualsZero ()
	{
		for ( int i = 0; i < numRows; i++ )
			for ( int j = 0; j < numCols; j++ )
				if ( get ( i, j ) != 0 )
					return false;
		return true;
	}
}