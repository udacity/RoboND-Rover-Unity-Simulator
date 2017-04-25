using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSolver : MonoBehaviour {
	/**
     *  6 x nJoints Matrix
     *  6 degrees of freedom
     * A collection of partial derivatives that describes
     * how the end effector changes position and orientation
     * with respect to changes in joint[i] orientation changes.
     * 
     *  Matrix = 6 x numJoints
     *           [ x0 -> ...  ]
     *           [ y0 --> ... ]
     *           [ z0 --> ... ]
     *  Matrix = [ a0 --> ... ] , where a0,a1,a2 are theta orientation, matrix is column major
     *           [ a1 --> ... ]   describing each joint
     *           [ a2 --> ... ]
     *
     *
     * 
     *  6 is the number of Degree of freedom for each joint.
     * 3 for position and 3 for orientation
     */
		public DynamicMatrix m_Jacobian = null;
		/**
	 * Describes the inverse of Jacobian.  Used to 
	 * give the needed changes in orientation that effectuates
	 * end effector to reach goal.
	 * 
	 * Matrix = numJoints x 6
     *
     *
     * Matrix = [  a  |
     *              \|/ ]
     *
     *
     *
	 */
	public DynamicMatrix m_JacobianTranspose = null;

	/**
     *   6 x 1 Matrix
     * Holds the directional pull of the end effector 
     * towards the goal.
     */
	public DynamicMatrix m_farrError = null;
	/**
     * A vector of joint velocities.  Each entry in this
     * matrix is representative of the angle (in radians)
     * that cause the End Effector to reach the target (goal).
     */
	public DynamicMatrix m_QDerivative = null;	//Joint rotation angles
	/**
     * For each joint in the IKChain, the axis around which the
     * joint rotates about is calculated and saved.  This 
     * axis is derived from;
     * 
     * 
     *
     *
     *     Array of Axis where each entry represents joint[i] rotation axis
     *     axis = &#x3C;i>Normalise&#x3C;/i>( (effector - joint_i) x (goal - joint_i) )
     *
     *  
     */
	public Vector3[] m_arrAxis				 	= null;
	/**
     * Indicates the number of possible interactions
     * that should be attempted and continued until the
     * IKSolver is forced to terminate, whether a solution
     * (end effector reaches the goal) was found or not.
     */
	public int 			m_iTryLimit		 ;
	/**
     * Describes the acceptable distance between the
     * end effector and the target goal.  In short,
     * used to terminate the IKSolver iteration if 
     * the distance between target and end effector
     * is less than this value.
     */
	public float 		m_fTargetThreshold 	 ;

	/**
     * &#x3C;p>Overall, this is the step rate for the changes in orientation&#x3C;/p>
     * &#x3C;p>
     * Describes how quickly the derived angles will rotate in
     * attempts to cause the end effector to reach the target.&#x3C;/p>
     * 
     * &#x3C;p>
     * The Smaller the value, the more smooth and controlled the 
     * transition will be towards the target.  The larger the value
     * the more erratic the transition will be towards the target.&#x3C;/p>
     */

		public float integration = 0.0005f;
	/**
     * The number of times the IKSolver will iterate before 
     * forcing stop of execution of the jacobian.  
     *
     */
	public int iTries	= 0;

	public IKSolver(int nJoints){
			m_Jacobian = new 	DynamicMatrix(6,nJoints);
			m_farrError              = new	DynamicMatrix(6,1);
			m_QDerivative = new 	DynamicMatrix(nJoints,1);
			m_arrAxis				 = new  Vector3[nJoints];

			iTries 					 = 0;
		m_fTargetThreshold 		= 1.5f;
		m_iTryLimit			 	= 4;
	}
}